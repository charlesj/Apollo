import { useContext } from 'react'
import { getDataContext } from './DataProvider'
import { coerceDataToImmutable } from '../util/coerceDataToImmutable'

export const useData = (path, notSetValue) => {
  const { state, setState } = useContext(getDataContext())

  const value = state.getIn(path, notSetValue)

  const set = updatedValue => {
    setState(current => current.setIn(path, coerceDataToImmutable(updatedValue)))
  }

  return [value, set]
}
