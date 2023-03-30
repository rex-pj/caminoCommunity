const LoadablePlugin = require("@loadable/webpack-plugin");
const HtmlWebpackPlugin = require("html-webpack-plugin");
const path = require("path");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const Dotenv = require("dotenv-webpack");
const InterpolateHtmlPlugin = require("interpolate-html-plugin");
const { WebpackManifestPlugin } = require("webpack-manifest-plugin");
const JsonMinimizerPlugin = require("json-minimizer-webpack-plugin");
const CopyPlugin = require("copy-webpack-plugin");

module.exports = (e, argv) => {
  let envMode = "";
  if (argv.nodeEnv && argv.nodeEnv !== "none") {
    envMode = `.${argv.nodeEnv}`;
  }

  const destDir = "build";
  const outputPath = path.resolve(__dirname, destDir);
  const assetsDir = "assets";
  return {
    mode: argv.nodeEnv,
    entry: "./src/index.js",
    output: {
      path: outputPath,
      filename: "bundle.js",
      publicPath: "/",
      clean: true,
    },
    devServer: {
      port: 3000,
      historyApiFallback: true,
      hot: false,
      webSocketServer: false,
      server: {
        type: "https",
        options: {
          key: "./.cert/localhost-key.pem",
          cert: "./.cert/localhost.pem",
        },
      },
    },
    module: {
      rules: [
        {
          test: /\.(js|jsx)$/,
          exclude: /node_modules/,
          use: ["babel-loader"],
        },
        {
          test: /\.(png|svg|jpg|jpeg|gif)$/i,
          type: "asset/resource",
          generator: {
            filename: `${assetsDir}/images/[name][ext]`,
          },
        },
        {
          test: /\.json$/i,
          type: "asset/resource",
        },
        {
          test: /\.css$/i,
          use: ["style-loader", "css-loader"],
        },
      ],
    },
    resolve: {
      extensions: ["*", ".js", ".jsx"],
    },
    plugins: [
      new LoadablePlugin(),
      new HtmlWebpackPlugin({
        template: "./public/index.html",
        favicon: "./public/favicon.ico",
        publicPath: "/",
        title: argv.nodeEnv,
      }),
      new InterpolateHtmlPlugin({
        PUBLIC_URL: `/${assetsDir}/`,
      }),
      new JsonMinimizerPlugin(),
      new CopyPlugin({
        patterns: [
          {
            context: path.resolve(__dirname),
            from: `./src/assets/locales/**/*.json`,
            to({ context, absoluteFilename }) {
              let destPath = absoluteFilename.replace(context, "");
              destPath = destPath.replace("src", destDir);
              destPath = `${context}${destPath}`;
              return destPath;
            },
          },
        ],
      }),
      new WebpackManifestPlugin({
        basePath: assetsDir,
        fileName: `${assetsDir}/manifest.json`,
        seed: {
          short_name: "Camino",
          name: "Camino Community",
          icons: [
            {
              src: `favicon.ico`,
              sizes: "64x64 32x32 24x24 16x16",
              type: "image/x-icon",
            },
          ],
          start_url: ".",
          display: "standalone",
          theme_color: "#2E420E",
          background_color: "#F8FBF1",
        },
      }),
      new MiniCssExtractPlugin(),
      new Dotenv({
        systemvars: true,
        path: `./environments/.env${envMode}`,
        safe: "./environments/.env",
      }),
    ],
    devtool: "eval-cheap-module-source-map",
  };
};
