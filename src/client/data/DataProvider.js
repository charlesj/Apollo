import React, { useState } from 'react'
import { load } from './storage'
const DataContext = React.createContext()

export const DataProvider = ({ children }) => {
  const [state, setState] = useState(load())

  return (
    <DataContext.Provider value={{ state, setState }}>
      {children}
    </DataContext.Provider>
  )
}

export function getDataContext() {
  return DataContext
}

export function getDataConsumer() {
  return DataContext.Consumer
}
