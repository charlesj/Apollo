import React from 'react'
import CssBaseline from '@material-ui/core/CssBaseline'
import { ThemeProvider } from '@material-ui/styles'
import { DataProvider } from './data/DataProvider'
import { theme } from './styles/theme'
import App from './App'
import AppHelper from './data/AppHelper'

const Frame = () => {
  return (
    <DataProvider>
      <AppHelper />
      <ThemeProvider theme={theme}>
        <CssBaseline />
        <App />
      </ThemeProvider>
    </DataProvider>
  )
}

export default Frame
