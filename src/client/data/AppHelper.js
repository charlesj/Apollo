import React, { useContext, useEffect } from 'react'
import { getDataContext } from './DataProvider'

const AppHelper = () => {
  const { state } = useContext(getDataContext())
  useEffect(() => {
    window.getData = () => state.toJS()
    return () => window.getData = undefined
  }, [state])

  return <></>
}

export default AppHelper
