const path = require('path')
const HtmlWebpackPlugin = require('html-webpack-plugin')
const CleanWebpackPlugin = require('clean-webpack-plugin')

const clientBuildFolder = '../build/client/'

module.exports = {
  entry: ['@babel/polyfill', './src/client/index.js'],
  devServer: {
    contentBase: clientBuildFolder,
    port: 3042,
    historyApiFallback: true,
  },
  plugins: [
    new CleanWebpackPlugin([clientBuildFolder]),
    new HtmlWebpackPlugin({
      title: 'apollo',
      hash: true,
    }),
  ],
  performance: { hints: false },
  output: {
    filename: 'main.js',
    path: path.resolve(__dirname, clientBuildFolder),
    publicPath: '/',
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
      {
        test: /\.(png|jpg|gif)$/,
        use: [
          {
            loader: 'file-loader',
            options: {}
          }
        ]
      }
    ]
  }
}
