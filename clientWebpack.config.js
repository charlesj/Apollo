const path = require('path')
const HtmlWebpackPlugin = require('html-webpack-plugin')
const { CleanWebpackPlugin } = require('clean-webpack-plugin')
const CopyWebpackPlugin = require('copy-webpack-plugin')
const webpack = require('webpack')

const clientBuildFolder = 'build/client/'

const quote = (val) => `'${val}'`

module.exports = {
  entry: ['./src/client/index.js'],
  devServer: {
    contentBase: `./${clientBuildFolder}`,
    port: 3042,
    historyApiFallback: true,
  },
  node: { fs: 'empty' },
  plugins: [
    new CleanWebpackPlugin({ cleanOnceBeforeBuildPatterns: [clientBuildFolder] }),
    new HtmlWebpackPlugin({
      title: 'Apollo',
      favicon: './src/client/public/favicon.ico',
      hash: true,
    }),
    new webpack.DefinePlugin({
      'environment': {
        API_SERVER: process.env.API_SERVER ? quote(process.env.API_SERVER) : quote('http://localhost:8042'),
      }
    }),
    new CopyWebpackPlugin(['./src/react-server.js'])
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
        test: /\.css$/,
        use: ['style-loader', 'css-loader'],
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
