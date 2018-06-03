const express = require("express");
const actionsOnGoogleAdapter = require("bot-framework-actions-on-google");

const app = express();

app.use(actionsOnGoogleAdapter("KiMXfpFG9fI.cwA.wc8.lr9uc81PSOXeWEmmDg6TE7XmH9-LxLiuY6h1UUts_uM").router);

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`ActionsOnGoogle demo listening on port ${PORT}!`));