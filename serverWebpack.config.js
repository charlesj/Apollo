const path = require('path')
const { CleanWebpackPlugin } = require('clean-webpack-plugin')
const nodeExternals = require('webpack-node-externals')

const serverBuildFolder = 'build/server/'

module.exports = {
  entry: ['./src/server/index.js'],
  plugins: [
    new CleanWebpackPlugin({ cleanOnceBeforeBuildPatterns: [serverBuildFolder] }),
  ],
  target: 'node',
  externals: [nodeExternals()],
  output: {
    filename: 'main.js',
    path: path.resolve(__dirname, serverBuildFolder),
  },
  module: {
    rules: [
      {
        test: /\.js$/,
        exclude: /node_modules/,
        use: {
          loader: 'babel-loader'
        }
      },
    ]
  }
}
