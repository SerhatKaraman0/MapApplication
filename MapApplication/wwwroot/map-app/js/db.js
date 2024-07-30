var pg = require("pg");

var connectionString = "postgres://postgres:1234@localhost:5432/queries";
var pgClient = new pg.Client(connectionString);

pgClient.connect((err) => {
  if (err) {
    return console.error("could not connect to postgres", err);
  }
  console.log("Connected to PostgreSQL");
});

function createQuery(queryName, queryDesc, query_q) {
  const queryString =
    "INSERT INTO queries (query_name, query_description, query_q) VALUES ($1, $2, $3) RETURNING *";
  const values = [queryName, queryDesc, query_q];

  return new Promise((resolve, reject) => {
    pgClient.query(queryString, values, (err, result) => {
      if (err) {
        console.error("Error running query", err);
        return reject(err);
      }
      console.log(result.rows); // Logs the inserted rows
      resolve(result.rows); // Returns the inserted rows
    });
  });
}

// Usage example
createQuery("Test Query", "This is a test description", "SELECT * FROM table;")
  .then((rows) => {
    console.log(rows); // Should log an array of inserted rows
    pgClient.end(); // Close the connection after operation
  })
  .catch((err) => {
    console.error("Error inserting query:", err);
    pgClient.end(); // Close the connection in case of error as well
  });
