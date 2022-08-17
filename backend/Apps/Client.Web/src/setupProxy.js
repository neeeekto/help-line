const { createProxyMiddleware } = require("http-proxy-middleware");

module.exports = function (app) {
  app.use(
    "/api",
    createProxyMiddleware({
      target: "http://127.0.0.1:5001",
      changeOrigin: true,
      pathRewrite: {
        "^/api": "",
      },
    })
  );

  app.use(
    createProxyMiddleware("/hubs", {
      target: "http://127.0.0.1:5001",
      changeOrigin: true,
      ws: true,
    })
  );
};
