
/* global window */
import React from 'react'
import ReactDOM from 'react-dom'
import Frame from './Frame'
import consoleHelpers from './consoleHelpers'
import { configureBasic, logger } from '../logging'

window.app = consoleHelpers

function main() {
  configureBasic()
  const root = document.createElement('div') // eslint-disable-line no-undef
  document.body.appendChild(root) // eslint-disable-line no-undef
  ReactDOM.render(<Frame />, root)
  logger.info('app loaded')
}

main()
