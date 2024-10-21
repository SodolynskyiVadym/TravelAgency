let serverUrl;
if (process.env.ENVIROMENT === "DockerEnv") {
    serverUrl = process.env.SERVER_ADDRESS;
}else if (process.env.ENVIROMENT === "Production") {
    serverUrl = process.env.SERVER_ADDRESS;
}else {
    serverUrl = process.env.SERVER_DEFAULT_ADDRESS || "http://localhost:5160";
}

export default serverUrl;