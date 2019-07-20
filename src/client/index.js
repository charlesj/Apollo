
/* global window */
import React from 'react'
import ReactDOM from 'react-dom'
import App from './App'
import consoleHelpers from './consoleHelpers'
import { configureBasic, logger } from '../logging'

window.app = consoleHelpers

function main() {
  configureBasic()
  const root = document.createElement('div') // eslint-disable-line no-undef
  document.body.appendChild(root) // eslint-disable-line no-undef
  ReactDOM.render(<App />, root)
  logger.info('app loaded')
}

main()
