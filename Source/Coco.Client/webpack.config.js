const LoadablePlugin = require("@loadable/webpack-plugin");

module.exports = {
  // ...
  plugins: new LoadablePlugin(),
  entry: ["@babel/polyfill", "./src/index.js"],
  resolve: {
    extensions: [".js", ".jsx"]
  }
};
