let serverUrl;
if (process.env.SERVER_ADDRESS && process.env.SERVER_PORT) {
    serverUrl = `${process.env.SERVER_ADDRESS}:${process.env.SERVER_PORT}`;
} else {
    serverUrl = process.env.SERVER_DEFAULT_ADDRESS || "http://localhost:5113";
}

export default serverUrl;