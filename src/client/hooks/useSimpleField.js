import { useState } from 'react'

export const useSimpleField = (label, initialValue = '') => {
  const [value, updateValue] = useState(initialValue)

  const onChange = ({ target: { value: newValue } }) => updateValue(newValue)

  return {
    label,
    value,
    onChange,
  }
}
