const baseURL = "http://localhost:5160";

async function getAllWkt() {
  const endpoint = baseURL + "/api/Values/wkt/all";
  try {
    const response = await fetch(endpoint);
    if (!response.ok) {
      throw new Error(`Response status: ${response.status}`);
    }
    const json = await response.json();
    console.log(json);
    return json;
  } catch (error) {
    console.log("Error fetching data: " + error);
  }
}

async function getWktById(id) {
  const endpoint = baseURL + `/api/Values/wkt/${id}`;
  try {
    const response = await fetch(endpoint);
    if (!response.ok) {
      throw new Error(`Response status: ${response.status}`);
    }
    const json = await response.json();
    console.log(json);
    return json;
  } catch (error) {
    console.log("Error fetching data: " + error);
  }
}


async function createWkt(name, description, wkt, photoLocation, color) {

  const endpoint = baseURL + `/api/Values/wkt/create`;
  const payload = {
    Name: name,
    Description: description,
    WKT: wkt,
    PhotoLocation: photoLocation,
    Color: color,
  };
  try {
    const response = await fetch(endpoint, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(payload),
    });

    if (!response.ok) {
      throw new Error(`Response status: ${response.status}`);
    }
    const json = await response.json();
    console.log(json);
    return json;
  } catch (error) {
    console.log("Error creating data: " + error);
  }
}

async function updateWkt(id, name, description, wkt, photoLocation, color) {
  const fetchEndpoint = baseURL + `/api/Values/wkt/${id}`;
  const updateEndpoint = baseURL + `/api/Values/wkt/update/${id}`;

  try {
    // Fetch current data
    const fetchResponse = await fetch(fetchEndpoint, {
      method: "GET",
      headers: {
        Accept: "application/json",
      },
    });

    if (!fetchResponse.ok) {
      throw new Error(`Fetch response status: ${fetchResponse.status}`);
    }

    const currentData = await fetchResponse.json();

    // Merge current data with updated fields
    const updatedWkt = {
      Id: currentData.Id, // Preserving the original ID
      Name: name !== undefined ? name : currentData.Name,
      Description: description !== undefined ? description : currentData.Description,
      WKT: wkt !== undefined ? wkt : currentData.WKT,
      PhotoLocation: photoLocation !== undefined ? photoLocation : currentData.PhotoLocation,
      Color: color !== undefined ? color : currentData.Color,
    };

    // Send the updated data to the server
    const updateResponse = await fetch(updateEndpoint, {
      method: "PUT",
      headers: {
        Accept: "text/plain",
        "Content-Type": "application/json",
      },
      body: JSON.stringify(updatedWkt),
    });

    if (!updateResponse.ok) {
      throw new Error(`Update response status: ${updateResponse.status}`);
    }

    const json = await updateResponse.json();
    console.log(json);
    return json;
  } catch (error) {
    console.log("Error updating data: " + error);
  }
}


async function deleteWkt(id) {
  const endpoint = baseURL + `/api/Values/wkt/delete/${id}`;

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

    const json = await response.json();
    console.log(json);
    return json;
  } catch (error) {
    console.log("Error deleting data: " + error);
  }
}

export { getAllWkt, getWktById, createWkt, updateWkt, deleteWkt };
