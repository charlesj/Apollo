/* global window */
import React from 'react'
import ReactDOM from 'react-dom'
import App from './App'
// import consoleHelpers from './consoleHelpers'
// import { configureForClient, logger } from '../logger'

// window.app = consoleHelpers

function main() {
  //configureForClient()
  const root = document.createElement('div') // eslint-disable-line no-undef
  document.body.appendChild(root) // eslint-disable-line no-undef
  ReactDOM.render(<App />, root)
  //logger.info('app loaded')
}

main()
