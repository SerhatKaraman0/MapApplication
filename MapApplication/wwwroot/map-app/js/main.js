import "../css/style.css";
import { Map, View } from "ol";
import TileLayer from "ol/layer/Tile.js";
import OSM from "ol/source/OSM.js";
import { Icon, Style } from "ol/style.js";
import Feature from "ol/Feature.js";
import Point from "ol/geom/Point.js";
import VectorSource from "ol/source/Vector.js";
import VectorLayer from "ol/layer/Vector.js";
import Overlay from "ol/Overlay.js";
import { Modify } from "ol/interaction.js";
import Collection from "ol/Collection";
import * as Api from "./api_operations.js";
import DataTable from "datatables.net-dt";
import $ from "jquery";
import { fromLonLat } from "ol/proj.js";
import { jsPanel } from "jspanel4";
import Draw from "ol/interaction/Draw.js";

// script.js
const selectedAction = [];

async function showAlert(points) {
  const alertContainer = document.getElementById("alert-container");

  if (!alertContainer) {
    console.error("Alert container not found.");
    return;
  }

  // Create alert box
  const alertBox = document.createElement("div");
  alertBox.className = "alert";

  // Create close button
  const closeButton = document.createElement("span");
  closeButton.className = "closebtn";
  closeButton.innerHTML = "&times;";
  closeButton.onclick = function () {
    this.parentElement.style.display = "none";
  };

  // Add message to alert box
  await Promise.all(
    ((alertBox.textContent = points.responseMessage),
    (alertBox.className = points.success ? "alert success" : "alert fail"))
  );

  // Append close button to alert box
  alertBox.appendChild(closeButton);

  // Append alert box to container
  alertContainer.appendChild(alertBox);

  // Set timer to remove alert box
  setTimeout(() => {
    alertBox.classList.add("fade-out");
    alertBox.addEventListener("transitionend", () => {
      alertBox.remove();
    });
  }, 3000);
}

function showAlertMode(points) {
  const alertContainer = document.getElementById("alert-container");

  if (!alertContainer) {
    console.error("Alert container not found.");
    return;
  }

  // Create alert box
  const alertBox = document.createElement("div");
  alertBox.className = "alert";

  // Create close button
  const closeButton = document.createElement("span");
  closeButton.className = "closebtn";
  closeButton.innerHTML = "&times;";
  closeButton.onclick = function () {
    this.parentElement.style.display = "none";
  };

  // Add message to alert box

  alertBox.textContent = points.responseMessage;
  alertBox.className = "alert message";

  // Append close button to alert box
  alertBox.appendChild(closeButton);

  // Append alert box to container
  alertContainer.appendChild(alertBox);

  // Set timer to remove alert box
  setTimeout(() => {
    alertBox.classList.add("fade-out");
    alertBox.addEventListener("transitionend", () => {
      alertBox.remove();
    });
  }, 3000);
}

const centerCoordinates = [3917151.932317253, 4770232.626187268];
let markers = [];

const markerLayer = new VectorLayer({
  source: new VectorSource({ wrapX: false }),
  style: new Style({
    image: new Icon({
      anchor: [0.5, 0.7],
      src: "/assets/pin.png",
      scale: 0.08,
      opacity: 0.8,
    }),
  }),
});

const selectedPointLayer = new VectorLayer({
  source: new VectorSource(),
  style: new Style({
    image: new Icon({
      anchor: [0.5, 0.7],
      src: "/assets/location-pin.svg",
      scale: 5,
      opacity: 0.8,
    }),
  }),
});

const map = new Map({
  target: "map",
  layers: [
    new TileLayer({
      source: new OSM(),
    }),
    markerLayer,
  ],
  view: new View({
    center: centerCoordinates,
    zoom: 6.5,
    minZoom: 6.5,
    maxZoom: 13,
    constrainResolution: true,
  }),
});

const sidebar = document.getElementById("sidebar");
const addPointBtn = document.querySelector(".add-point-btn");
const queryBtn = document.querySelector(".query-btn");
const closeSidebar = document.querySelector(".close-sidebar");
const addPointButton = document.querySelector(".save-btn");

let clickedCoordinates = null;
let selectedFeature = null;

function openSidebar(formId) {
  sidebar.style.width = "30%";
  document.body.classList.add("sidebar-open");
  document.querySelectorAll(".form-section").forEach((section) => {
    section.style.display = "none";
  });
  document.getElementById(formId).style.display = "block";
  setTimeout(() => map.updateSize(), 500);
}

function closeSidebarFunction() {
  sidebar.style.width = "0";
  document.body.classList.remove("sidebar-open");
  setTimeout(() => map.updateSize(), 500);
}

function createTemporaryMarker(coordinate) {
  const tempFeature = new Feature({
    geometry: new Point(coordinate),
  });
  tempFeature.set("class", "temp");
  markerLayer.getSource().addFeature(tempFeature);
  return tempFeature;
}

function createSavedMarker(coordinates, name, id) {
  const savedFeature = new Feature({
    geometry: new Point(coordinates),
  });

  savedFeature.setId(id); // Set a unique ID for the feature
  savedFeature.set("class", "saved");
  savedFeature.set("name", name);
  savedFeature.setStyle(
    new Style({
      image: new Icon({
        anchor: [0.5, 0.7],
        src: "/assets/saved-location-from-db.png",
        scale: 0.07,
        opacity: 0.8,
        className: "bounce",
      }),
    })
  );
  markers.push(savedFeature);
  markerLayer.getSource().addFeature(savedFeature);

  // Add the feature to the modify interaction's collection
  featureCollection.push(savedFeature);

  return savedFeature;
}

async function createAllSavedMarkers(points) {
  clearAllMarkers();
  await Promise.all(
    points.point.map(async (point) => {
      createSavedMarker(
        [point.x_coordinate, point.y_coordinate],
        point.name,
        point.id
      );
    })
  );
}

function removeTempMarkers() {
  const source = markerLayer.getSource();
  const features = source.getFeatures();
  features.forEach((feature) => {
    if (feature.get("class") === "temp") {
      source.removeFeature(feature);
    }
  });
}

function removeSavedMarkers() {
  const source = markerLayer.getSource();
  const features = source.getFeatures();
  features.forEach((feature) => {
    if (feature.get("class") === "saved") {
      source.removeFeature(feature);
    }
  });
}

function clearAllMarkers() {
  removeTempMarkers();
  removeSavedMarkers();
  markers.forEach((marker) => map.removeLayer(marker));
  markers = [];
}

function savePoint() {
  if (selectedFeature) {
    const name = document.querySelector("#point_name").value;
    selectedFeature.set("name", name);
    selectedFeature.setStyle(
      new Style({
        image: new Icon({
          anchor: [0.5, 0.7],
          src: "/assets/location-pin.svg",
          scale: 0.05,
          opacity: 0.8,
        }),
      })
    );

    // Update the coordinates of the marker
    const coords = selectedFeature.getGeometry().getCoordinates();
    selectedFeature.set("x_coordinate", coords[0]);
    selectedFeature.set("y_coordinate", coords[1]);

    if (selectedFeature.get("class") === "temp") {
      selectedFeature.set("class", "saved");
      markers.push(selectedFeature);
    }

    selectedFeature = null; // Reset selectedFeature after saving
    closeSidebarFunction();
  }
}

addPointBtn.addEventListener("click", () => openSidebar("add-point-form"));
queryBtn.addEventListener("click", () => openSidebar("query-form"));
closeSidebar.addEventListener("click", closeSidebarFunction);
addPointButton.addEventListener("click", savePoint);

function useClickedCoordinates() {
  if (clickedCoordinates) {
    console.log(
      "Accessing clicked coordinates in another function:",
      clickedCoordinates
    );
  } else {
    console.log("No coordinates available yet.");
  }
}

async function panAndZoomTo(id, zoomLevel = 14, duration = 2000) {
  // Retrieve the view from the map object
  const view = map.getView();

  try {
    // Get the point data
    const point = await Api.getPointById(id);

    if (point && point.point[0].x_coordinate && point.point[0].y_coordinate) {
      const xCoordinate = point.point[0].x_coordinate;
      const yCoordinate = point.point[0].y_coordinate;

      if (
        !(
          xCoordinate === 3917151.932317253 && yCoordinate === 4770232.626187268
        )
      ) {
        const centerCoordinates = [3917151.932317253, 4770232.626187268];
        const location = [xCoordinate, yCoordinate];

        // Animate to initial center
        view.animate(
          {
            center: centerCoordinates,
            zoom: 6.5,
            duration: duration,
          },
          function () {
            view.animate({
              center: location,
              zoom: zoomLevel,
              duration: duration,
            });
          }
        );
      } else {
        const location = [xCoordinate, yCoordinate];

        view.animate({
          center: location,
          zoom: zoomLevel,
          duration: duration,
        });
      }
    } else {
      console.error("Point data is missing or invalid.");
      console.log(point.point[0].x_coordinate, point.point[0].y_coordinate);
    }
  } catch (error) {
    console.error("Error fetching point data:", error);
  }
}

window.panAndZoomTo = panAndZoomTo;

async function updateFromTable(id) {
  await panAndZoomTo(id, 14, 2000);

  if (window.currentPanel) {
    window.currentPanel.close();
  }

  const view = map.getView();

  function createPanel() {
    window.currentPanel = jsPanel.create({
      headerTitle: `Update Point: ${id}`,
      contentSize: "400 200",
      content: `
        <div style="padding: 10px;">
          <div style="margin-bottom: 10px;">
            <label for="update-point-x-popup" style="display: block; font-weight: bold; margin-bottom: 5px;">X Coordinate:</label>
            <input type="text" id="update-point-x-popup" style="width: 100%; padding: 5px; border: 1px solid #ccc; border-radius: 3px;" />
          </div>
          <div style="margin-bottom: 10px;">
            <label for="update-point-y-popup" style="display: block; font-weight: bold; margin-bottom: 5px;">Y Coordinate:</label>
            <input type="text" id="update-point-y-popup" style="width: 100%; padding: 5px; border: 1px solid #ccc; border-radius: 3px;" />
          </div>
          <div style="margin-bottom: 10px;">
            <label for="update-point-name-field-popup" style="display: block; font-weight: bold; margin-bottom: 5px;">Name:</label>
            <input type="text" id="update-point-name-field-popup" style="width: 100%; padding: 5px; border: 1px solid #ccc; border-radius: 3px;" />
          </div>
          <button class="btn save-btn" id="update-point-btn-popup" style="width: 100%; padding: 10px; background-color: #007bff; color: white; border: none; border-radius: 3px; cursor: pointer;">Apply</button>
        </div>
      `,
      position: "center-top 0 400",
      theme: "dark",
      panelSize: {
        width: "400px",
        height: "350px",
      },
      callback: function (panel) {
        document
          .querySelector("#update-point-btn-popup")
          .addEventListener("click", async () => {
            const point_x_coordinate = document.querySelector(
              "#update-point-x-popup"
            ).value;
            const point_y_coordinate = document.querySelector(
              "#update-point-y-popup"
            ).value;
            const point_name = document.querySelector(
              "#update-point-name-field-popup"
            ).value;

            const response = await Api.updatePoint(
              id,
              point_x_coordinate,
              point_y_coordinate,
              point_name
            );

            $("#query-table").DataTable().ajax.reload(null, false);

            showAlert(response);

            clearAllMarkers();

            createAllSavedMarkers(response);
          });
      },
    });
  }

  function onAnimationEnd() {
    createPanel();
    view.un("change:center", onAnimationEnd);
    view.un("change:resolution", onAnimationEnd);
  }

  view.on("change:center", onAnimationEnd);
  view.on("change:resolution", onAnimationEnd);
}

window.updateFromTable = updateFromTable;

async function deleteFromTable(id) {
  const response = await Api.deletePoint(id);

  $("#query-table").DataTable().ajax.reload(null, false);

  showAlert(response);
}

window.deleteFromTable = deleteFromTable;

function deleteHtmlElement(id) {
  const element = document.querySelector(id);
  element.remove();
}

useClickedCoordinates();

document.addEventListener("DOMContentLoaded", async () => {
  const points = await Api.getAllPoints();
  showAlert(points);
  clearAllMarkers();

  createAllSavedMarkers(points);

  document
    .getElementById("expandButton")
    .addEventListener("click", function () {
      const container = document.querySelector(
        ".floating-action-btn-container"
      );
      container.classList.toggle("show");
    });

  const buttons = document.querySelectorAll(
    ".floating-action-btn-container button"
  );

  let modifyInteraction;
  let modify;

  let mapClickHandler;
  let drawModifier;

  function enableDrawPolygonAction() {
    const source = markerLayer.getSource();
    drawModifier = new Draw({
      source: source,
      type: "Polygon",
    });

    map.addInteraction(drawModifier);
  }

  function disableDrawPolygonAction() {}

  function enableDrawLineAction() {}

  function disableDrawLineAction() {}

  function enableDrawCircleAction() {}

  function disableDrawCircleAction() {}

  function onMapClick(coordinate) {
    clickedCoordinates = coordinate;
    document.querySelector("#x_coordinate").value = coordinate[0];
    document.querySelector("#y_coordinate").value = coordinate[1];
    openSidebar("add-point-form");

    // Remove all temporary markers
    removeTempMarkers();

    // Create a new temporary marker
    selectedFeature = createTemporaryMarker(coordinate);
  }

  // Function to enable map click
  function enableMapClick() {
    mapClickHandler = (e) => {
      onMapClick(e.coordinate);
    };
    map.on("click", mapClickHandler);
  }

  // Function to disable map click
  function disableMapClick() {
    if (mapClickHandler) {
      map.un("click", mapClickHandler);
      mapClickHandler = null;
    }
  }

  function enableDragAndDropAction() {
    modify = new Modify({
      // The source where features are added or modified
      source: markerLayer.getSource(),
      // Optional: Use this to define how to handle the hit detection
      hitDetection: markerLayer,
    });

    // Add the modify interaction to the map
    map.addInteraction(modify);

    const featureCollection = new Collection(markers);

    const modifyInteraction = new Modify({
      features: featureCollection, // Pass the Collection instance here
    });

    map.addInteraction(modifyInteraction);

    if (modifyInteraction) {
      map.removeInteraction(modifyInteraction); // Remove existing interaction if any
    }

    modify.on("modifystart", () => {
      map.getTargetElement().style.cursor = "grabbing";
    });

    modify.on("modifyend", async (event) => {
      console.log("Modify end event fired");
      const features = event.features.getArray();
      console.log("Features modified:", features);
      for (const feature of features) {
        if (feature && feature.get("class") === "saved") {
          const coordinates = feature.getGeometry().getCoordinates();
          const pointId = feature.getId(); // Get the feature ID
          const updatedX = coordinates[0];
          const updatedY = coordinates[1];
          const name = feature.get("name"); // Preserve the name of the point

          console.log(
            `Updating point ID ${pointId} to coordinates [${updatedX}, ${updatedY}]`
          );

          // Call the API to update the point
          try {
            const response = await Api.updatePoint(
              pointId,
              updatedX,
              updatedY,
              name
            );
            showAlert(response);
          } catch (error) {
            console.error("Error updating point:", error);
          }
        }
      }
    });
  }

  function disableDragAndDropAction() {
    map.getTargetElement().style.cursor = "";
    if (modify) {
      map.removeInteraction(modify);
      modify = null; // Clear the reference
    }

    // Remove the modify interaction for the feature collection
    if (modifyInteraction) {
      map.removeInteraction(modifyInteraction);
      modifyInteraction = null; // Clear the reference
    }
  }

  buttons.forEach((button) => {
    button.addEventListener("click", function () {
      if (this.classList.contains("active")) {
        this.classList.remove("active");
        const index = selectedAction.indexOf(this.id);
        if (index > -1) {
          selectedAction.splice(index, 1);
        }
      } else {
        buttons.forEach((btn) => {
          btn.classList.remove("active");
          const index = selectedAction.indexOf(btn.id);
          if (index > -1) {
            selectedAction.splice(index, 1);
          }
        });
        this.classList.add("active");
        selectedAction.push(this.id);
      }
      if (selectedAction.includes("drag-and-update-btn")) {
        enableDragAndDropAction();
      } else {
        disableDragAndDropAction();
      }

      if (selectedAction.includes("add-point-btn")) {
        enableMapClick();
      } else {
        removeTempMarkers();
        disableMapClick();
      }

      if (selectedAction.includes("draw-polygon-btn")) {
        enableDrawPolygonAction();
      }
    });
  });

  document.querySelector("#customButton").addEventListener("click", () => {
    const view = map.getView();
    const centerCoordinates = [3917151.932317253, 4770232.626187268];

    view.animate({
      center: centerCoordinates,
      zoom: 6.5,
      duration: 2000,
    });
  });

  const collapsibles = document.querySelectorAll(".collapsible");
  document
    .querySelector("#add-point-button-save")
    .addEventListener("click", async () => {
      // Get the values from the input fields
      var addPointX = parseFloat(document.querySelector("#x_coordinate").value);
      var addPointY = parseFloat(document.querySelector("#y_coordinate").value);
      var addPointName = document.querySelector("#point_name").value;

      // Ensure that x and y coordinates are numbers and valid
      if (isNaN(addPointX) || isNaN(addPointY)) {
        console.error("Invalid coordinates: X and Y must be numbers.");
        return;
      }

      // Call the API to create a new point
      const point = await Api.createPoint(addPointX, addPointY, addPointName);
      showAlert(point);
    });

  collapsibles.forEach((button) => {
    button.addEventListener("click", () => {
      // Toggle the "active" class to open/close the content
      button.classList.toggle("active");

      // Get the content element
      const content = button.nextElementSibling;

      // Check if the content is currently displayed
      if (content.style.maxHeight) {
        content.style.maxHeight = null;
      } else {
        content.style.maxHeight = content.scrollHeight + "px";
      }
    });
  });

  // Optional: Close sidebar when clicking the close button
  document.querySelector(".close-sidebar").addEventListener("click", () => {
    document.querySelector("#sidebar").style.width = "0";
    document.querySelector("#map").style.width = "100%"; // Reset map width
    document.body.classList.remove("sidebar-open");
  });

  // Optional: Show the sidebar when clicking the "Add Point" button
  document.querySelector(".add-point-btn").addEventListener("click", () => {
    document.querySelector("#sidebar").style.width = "30%"; // Adjust the width as needed
    document.querySelector("#map").style.width = "70%"; // Adjust the width as needed
    document.querySelector("#add-point-form").style.display = "block";
    document.querySelector("#query-form").style.display = "none";
    document.querySelector("#query-table-form").style.display = "none";
    document.body.classList.add("sidebar-open");
  });

  document.querySelector(".real-query-btn").addEventListener("click", () => {
    document.querySelector("#sidebar").style.width = "45%"; // Adjust the width as needed
    document.querySelector("#map").style.width = "55%"; // Adjust the width as needed
    document.querySelector("#add-point-form").style.display = "none";
    document.querySelector("#query-form").style.display = "none";
    document.querySelector("#query-table-form").style.display = "block";
    document.body.classList.add("sidebar-open");
  });

  // Optional: Show the query form when clicking the "Query" button
  document.querySelector(".query-btn").addEventListener("click", () => {
    document.querySelector("#sidebar").style.width = "30%"; // Adjust the width as needed
    document.querySelector("#map").style.width = "70%"; // Adjust the width as needed
    document.querySelector("#add-point-form").style.display = "none";
    document.querySelector("#query-form").style.display = "block";
    document.querySelector("#query-table-form").style.display = "none";
    document.body.classList.add("sidebar-open");
  });

  document
    .querySelector("#get-all-points-btn")
    .addEventListener("click", async () => {
      // Fetch points from the API
      const points = await Api.getAllPoints();
      showAlert(points);
      clearAllMarkers();

      createAllSavedMarkers(points);
    });

  const table = new DataTable("#query-table", {
    ajax: {
      url: "http://localhost:5160/api/Values",
      dataSrc: "point",
    },
    columns: [
      { data: "id" },
      { data: "name" },
      { data: "x_coordinate" },
      { data: "y_coordinate" },
      {
        data: null,
        render: function (data, type, row) {
          return `
                    <div class="query-row-btn-group">
                        <button class="query-table-btn view" 
                                data-id="${row.id}"
                                title="View this point">
                          <img class="query-table-btn-img" src="/assets/view-icon.png" border="0" />
                        </button>
                        <button class="query-table-btn update" 
                                title="Update this point" 
                                data-id="${row.id}">
                          <img class="query-table-btn-img" src="/assets/update-icon.png" border="0" />
                        </button>
                        <button class="query-table-btn delete" 
                                title="Delete this point" 
                                data-id="${row.id}">
                          <img class="query-table-btn-img" src="/assets/delete-icon.png" border="0" />
                        </button>
                    </div>
                    `;
        },
      },
    ],
  });

  document
    .getElementById("query-table")
    .addEventListener("click", async function (event) {
      if (event.target.closest(".query-table-btn.delete")) {
        const button = event.target.closest(".query-table-btn.delete");
        const id = parseInt(button.getAttribute("data-id"));
        if (!isNaN(id)) {
          await deleteFromTable(id);
        }
      }
    });

  document
    .getElementById("query-table")
    .addEventListener("click", async function (event) {
      if (event.target.closest(".query-table-btn.update")) {
        const button = event.target.closest(".query-table-btn.update");
        const id = parseInt(button.getAttribute("data-id"));
        if (!isNaN(id)) {
          await updateFromTable(id);
        }
      }
    });

  document
    .getElementById("query-table")
    .addEventListener("click", async function (event) {
      if (event.target.closest(".query-table-btn.view")) {
        const button = event.target.closest(".query-table-btn.view");
        const id = parseInt(button.getAttribute("data-id"));
        if (!isNaN(id)) {
          await panAndZoomTo(id, 14, 2000);
        }
      }
    });

  document
    .querySelector("#generate-points-btn")
    .addEventListener("click", async () => {
      const response = await Api.generatePoints();
      showAlert(response);
      clearAllMarkers();

      const points = await Api.getAllPoints();
      createAllSavedMarkers(points);
    });

  document
    .querySelector("#delete-all-points-btn")
    .addEventListener("click", async () => {
      clearAllMarkers();
      const response = await Api.deleteAll();
      showAlert(response);

      clearAllMarkers();

      const points = await Api.getAllPoints();
      createAllSavedMarkers(points);
    });

  document
    .querySelector("#update-point-btn")
    .addEventListener("click", async () => {
      var pointId = parseInt(document.querySelector("#update-point-id").value);
      var x_coordinate = parseFloat(
        document.querySelector("#update-point-x").value
      );
      var y_coordinate = parseFloat(
        document.querySelector("#update-point-y").value
      );
      var name = document.querySelector("#update-point-name-field").value;
      const response = await Api.updatePoint(
        pointId,
        x_coordinate,
        y_coordinate,
        name
      );
      showAlert(response);
    });

  document
    .querySelector("#update-point-name-btn")
    .addEventListener("click", async () => {
      var pointId = document.querySelector("#update-point-name-name").value;
      var x_coordinate = parseFloat(
        document.querySelector("#update-point-name-x").value
      );
      var y_coordinate = parseFloat(
        document.querySelector("#update-point-name-y").value
      );
      var name = document.querySelector("#update-point-updated-name").value;
      const response = await Api.updatePointByName(
        pointId,
        x_coordinate,
        y_coordinate,
        name
      );
      showAlert(response);
    });

  document
    .querySelector("#getPointByIdBtn")
    .addEventListener("click", async () => {
      var pointId = document.querySelector("#pointId").value;
      const point = await Api.getPointById(pointId);
      createAllSavedMarkers(point);
      showAlert(point);
    });

  document
    .querySelector("#getNearestPointBtn")
    .addEventListener("click", async () => {
      // Retrieve and parse the input values
      var x_coordinate = parseFloat(
        document.querySelector("#x_coordinate_nearest_point").value
      );
      var y_coordinate = parseFloat(
        document.querySelector("#y_coordinate_nearest_point").value
      );

      // Validate the coordinates
      if (isNaN(x_coordinate) || isNaN(y_coordinate)) {
        console.error("Invalid input values. Please enter valid numbers.");
        return;
      }

      // Call the API method to get the nearest point
      const point = await Api.getNearestPoint(x_coordinate, y_coordinate);
      createAllSavedMarkers(point);
      showAlert(point);
    });

  document.querySelector("#searchBtn").addEventListener("click", async () => {
    // Retrieve and parse the input values
    var x_coordinate = parseFloat(document.querySelector("#searchX").value);
    var y_coordinate = parseFloat(document.querySelector("#searchY").value);
    var range = parseFloat(document.querySelector("#range").value);

    // Validate the coordinates and range
    if (isNaN(x_coordinate) || isNaN(y_coordinate) || isNaN(range)) {
      console.error("Invalid input values. Please enter valid numbers.");
      return;
    }

    // Call the API method to perform the search
    const points = await Api.search(x_coordinate, y_coordinate, range);
    createAllSavedMarkers(points);
    showAlert(points);
  });

  document
    .querySelector("#deletePointBtn")
    .addEventListener("click", async () => {
      // Retrieve and parse the input value
      var pointId = parseInt(document.querySelector("#deleteId").value);

      // Validate the pointId
      if (isNaN(pointId)) {
        console.error("Invalid ID value. Please enter a valid number.");
        return;
      }

      // Call the API method to delete the point
      const point = await Api.deletePoint(pointId);
      showAlert(point);

      clearAllMarkers();

      const points = await Api.getAllPoints();
      createAllSavedMarkers(points);
    });

  /*
    document
    .querySelector("#getPointsInRadiusBtn")
    .addEventListener("click", async () => {
      var circleX = parseFloat(document.querySelector("#circleX").value);
      var circleY = parseFloat(document.querySelector("#circleY").value);
      var radius = parseFloat(document.querySelector("#radius").value);

      const points = await Api.getPointsInRadius(circleX, circleY, radius);
      showAlert(points);

      createAllSavedMarkers(points);
    });
      document.querySelector("#distanceBtn").addEventListener("click", async () => {
    var pointName1 = document.querySelector("#pointName1").value;
    var pointName2 = document.querySelector("#pointName2").value;
    const point = await Api.distance(pointName1, pointName2);
    showAlert(point);
  });
  document
    .querySelector("#deleteByRangeBtn")
    .addEventListener("click", async () => {
      // Retrieve and parse the input values
      var minX = parseFloat(document.querySelector("#minX").value);
      var minY = parseFloat(document.querySelector("#minY").value);
      var max_X = parseFloat(document.querySelector("#max_X").value);
      var maxY = parseFloat(document.querySelector("#maxY").value);

      // Validate the range values
      if (isNaN(minX) || isNaN(minY) || isNaN(max_X) || isNaN(maxY)) {
        console.error("Invalid input values. Please enter valid numbers.");
        return;
      }

      // Call the API method to delete by range
      const point = await Api.deleteByRange(minX, minY, max_X, maxY);
      showAlert(point);

      clearAllMarkers();

      const points = await Api.getAllPoints();
      createAllSavedMarkers(points);
    });

  document
    .querySelector("#deletePointByNameBtn")
    .addEventListener("click", async () => {
      var pointName = document.querySelector("#deleteName").value;
      const point = await Api.deletePointByName(pointName);
      showAlert(point);

      clearAllMarkers();

      const points = await Api.getAllPoints();
      createAllSavedMarkers(points);
    });
    */
});
