import express = require('express');
import { MintController } from "./api";
import path = require('path');
import dotenv = require('dotenv');

const app = express();
const PORT = process.env.PORT ?? 3000;

dotenv.config();

if (!process.env.OPENFORT_SECRET_KEY) {
  throw new Error(
    `Unable to load the .env file. Please copy .env.example to .env and fill in the required environment variables.`
  );
}

app.get("/protected-collect", (req, res) => {
  res.send("Service is running");
});
app.post('/mint', new MintController().run);
app.listen(PORT, () => {
  console.log(`Server is running at http://localhost:${PORT}`);
});