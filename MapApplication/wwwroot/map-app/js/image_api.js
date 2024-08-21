import axios from "axios";

async function getStaticImage(geojson) {
  const accessToken =
    "pk.eyJ1IjoibWFzdGVyLW9mLW5vbmUiLCJhIjoiY2x6OHhiZWU3MDZsNzJscXY5ZW92djRmMyJ9.8T-Z5F_eolNTQw2RM5z9jQ";
  const baseUrl =
    "https://api.mapbox.com/styles/v1/mapbox/streets-v12/static/geojson(";

  const geojsonString = encodeURIComponent(JSON.stringify(geojson));
  const url = `${baseUrl}${geojsonString})/auto/300x200?before_layer=admin-0-boundary-bg&access_token=${accessToken}`;

  try {
    const response = await axios.get(url);
    return response.config.url;
  } catch (error) {
    console.error("Error fetching static image:", error);
    throw error;
  }
}

export { getStaticImage };
