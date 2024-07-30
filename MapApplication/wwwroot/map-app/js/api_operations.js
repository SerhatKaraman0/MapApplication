const baseURL = "http://localhost:5160";

// GET Endpoints

async function getAllPoints() {
  const endpoint = baseURL + "/api/Values";

  try {
    const response = await fetch(endpoint); // Await the fetch call
    if (!response.ok) {
      throw new Error(`Response status: ${response.status}`); // Fixed error message
    }
    const json = await response.json(); // Await the JSON parsing
    console.log(json.point);
    return json;
  } catch (error) {
    console.log("Error fetching data:", error); // Added more descriptive error message
  }
}

async function generatePoints() {
  const endpoint = baseURL + "/api/Values/generate";
  try {
    const response = await fetch(endpoint);
    if (!response.ok) {
      throw new Error(`Response status: ${response.responseMessage}`);
    }
    const json = await response.json();
    console.log(json.point);
    return json;
  } catch (error) {
    console.log("Error fetching data:", error);
  }
}

async function getPointById(id) {
  const endpoint = baseURL + `/api/Values/${id}`;
  try {
    const response = await fetch(endpoint);
    if (!response.ok) {
      throw new Error(`Response status: ${response.responseMessage}`);
    }
    const json = await response.json();
    console.log(json.point);
    return json;
  } catch (error) {
    console.log("Error fetching data:", error);
  }
}

async function getPointsInRadius(circleX, circleY, radius) {
  const endpoint = `${baseURL}/api/Values/pointsInRadius?circleX=${circleX}&circleY=${circleY}&radius=${radius}`;
  try {
    const response = await fetch(endpoint);
    if (!response.ok) {
      throw new Error(
        `HTTP error! status: ${response.status}, message: ${response.statusText}`
      );
    }
    const json = await response.json();
    console.log(json.point); // Assuming json.point is the correct property
    return json;
  } catch (error) {
    console.error("Error fetching data:", error);
  }
}

async function getNearestPoint(X_coordinate, Y_coordinate) {
  const endpoint =
    baseURL + `/api/Values/getNearestPoint?X=${X_coordinate}&Y=${Y_coordinate}`;
  try {
    const response = await fetch(endpoint);
    if (!response.ok) {
      throw new Error(`Response status: ${response.responseMessage}`);
    }
    const json = await response.json();
    console.log(json.point);
    return json;
  } catch (error) {
    console.log("Error fetching data:", error);
  }
}

async function search(X_coordinate, Y_coordinate, range) {
  const endpoint =
    baseURL +
    `/api/Values/getNearestPoint?x=${X_coordinate}&y=${Y_coordinate}&range=${range}`;
  try {
    const response = await fetch(endpoint);
    if (!response.ok) {
      throw new Error(`Response status: ${response.responseMessage}`);
    }
    const json = await response.json();
    console.log(json.point);
    return json;
  } catch (error) {
    console.log("Error fetching data:", error);
  }
}

async function count() {
  const endpoint = baseURL + `/api/Values/count`;
  try {
    const response = await fetch(endpoint);

    if (!response.ok) {
      throw new Error(`Response status: ${response.responseMessage}`);
    }
    const json = await response.json();
    console.log(json);
    return json;
  } catch (error) {
    console.log("Error fetching data:", error);
  }
}

async function distance(pointName1, pointName2) {
  const endpoint =
    baseURL +
    `/api/Values/distance?pointName1=${pointName1}&pointName2=${pointName2}`;
  try {
    const response = await fetch(endpoint);

    if (!response.ok) {
      throw new Error(`Response status: ${response.responseMessage}`);
    }
    const json = await response.json();
    console.log(json);
    return json;
  } catch (error) {
    console.log("Error fetching data:", error);
  }
}

// End GET Endpoints

// POST Endpoints

async function createPoint(X, Y, name) {
  const endpoint = baseURL + `/api/Values`;

  const payload = {
    X_coordinate: X,
    Y_coordinate: Y,
    Name: name,
  };

  try {
    const response = await fetch(endpoint, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(payload), // Convert the point object to JSON
    });

    if (!response.ok) {
      throw new Error(`Response status: ${response.status}`);
    }

    const json = await response.json();
    console.log(json);
    return json;
  } catch (error) {
    console.log("Error adding point:", error);
  }
}

// End POST Requests

// PUT Requests

async function updatePoint(id, x, y, name) {
  const endpoint = `http://localhost:5160/api/Values/${id}`;
  const updatedPointPayload = {
    id: id,
    x_coordinate: x,
    y_coordinate: y,
    name: name,
  };

  try {
    const response = await fetch(endpoint, {
      method: "PUT",
      headers: {
        Accept: "text/plain", // This is equivalent to `-H 'accept: text/plain'` in `curl`
        "Content-Type": "application/json", // This is equivalent to `-H 'Content-Type: application/json'` in `curl`
      },
      body: JSON.stringify(updatedPointPayload), // This is equivalent to `-d` in `curl`
    });

    if (!response.ok) {
      const errorText = await response.text(); // Read the response text
      throw new Error(
        `Response status: ${response.status}, message: ${errorText}`
      );
    }
    const json = await response.json();
    console.log(json);
    return json;
  } catch (error) {
    console.error("Error updating point:", error);
  }
}

async function updatePointByName(nameId, X, Y, name) {
  const endpoint = baseURL + `/api/Values/updateByName/${nameId}`;

  const updatedPointPayload = {
    id: 0,
    x_coordinate: X,
    y_coordinate: Y,
    name: name,
  };
  try {
    const response = await fetch(endpoint, {
      method: "PUT",
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
      },
      body: JSON.stringify(updatedPointPayload), // Convert the payload object to JSON
    });

    if (!response.ok) {
      throw new Error(`Response status: ${response.status}`);
    }

    const jsonResponse = await response.json();
    console.log("Point updated successfully:", jsonResponse);
    return jsonResponse;
  } catch (error) {
    console.error("Error updating point:", error);
  }
}

// End PUT Requests

// DELETE Requests

async function deletePoint(id) {
  const endpoint = baseURL + `/api/Values/${id}`;

  try {
    const response = await fetch(endpoint, {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json",
      },
    });

    if (!response.ok) {
      throw new Error(`Response status: ${response.status}`);
    }

    const jsonResponse = await response.json();
    console.log("Point deleted successfully:", jsonResponse);
    return jsonResponse;
  } catch (error) {
    console.error("Error deleting point:", error);
  }
}

async function deletePointByName(name) {
  const endpoint = baseURL + `/api/Values/name/${name}`;

  try {
    const response = await fetch(endpoint, {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json",
      },
    });

    if (!response.ok) {
      throw new Error(`Response status: ${response.status}`);
    }

    const jsonResponse = await response.json();
    console.log("Point deleted successfully:", jsonResponse);
    return jsonResponse;
  } catch (error) {
    console.error("Error deleting point:", error);
  }
}

async function deleteByRange(minX, minY, max_X, maxY) {
  const endpoint =
    baseURL +
    `/api/Values/deleteByRange?minX=${minX}&minY=${minY}&max_X=${max_X}&maxY=${maxY}`;

  try {
    const response = await fetch(endpoint, {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json",
      },
    });

    if (!response.ok) {
      throw new Error(`Response status: ${response.status}`);
    }

    const jsonResponse = await response.json();
    console.log("Point deleted successfully:", jsonResponse);
    return jsonResponse;
  } catch (error) {
    console.error("Error deleting point:", error);
  }
}

async function deleteAll() {
  const endpoint = baseURL + `/api/Values/all`;

  try {
    const response = await fetch(endpoint, {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json",
      },
    });

    if (!response.ok) {
      throw new Error(`Response status: ${response.status}`);
    }

    const jsonResponse = await response.json();
    console.log("Point deleted successfully:", jsonResponse);
    return jsonResponse;
  } catch (error) {
    console.error("Error deleting point:", error);
  }
}

// End DELETE Requests

export {
  getAllPoints, // just button done
  generatePoints, // just button done
  getPointById, // id field done
  getPointsInRadius, // circleX, circleY, radius fields done
  getNearestPoint, // x_coordinate, y_coordinate fields done
  search, // x_coordinate, y_coordinate, range done
  count, // just button done
  distance, // pointName1, pointName2 fields done
  createPoint, // x, y, name fields done
  updatePoint, //id, x, y, name fields done
  updatePointByName, // nameId , x, y, name fields done
  deletePoint, // id fields done
  deletePointByName, // name fields done
  deleteByRange, // minX, minY, max_X, maxY fields done
  deleteAll, // just button x
};
