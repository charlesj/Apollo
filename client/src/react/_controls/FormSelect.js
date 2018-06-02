import React from 'react'
import PropTypes from 'prop-types'
import './FormSelect.css'

function FormSelect(props) {
  const { input, options, } = props

  return (
    <select {...input}>
      {options.map(opt => {
        return (
          <option key={opt.value} value={opt.value}>
            {opt.label}
          </option>
        )
      })}
    </select>
  )
}

FormSelect.propTypes = {
  input: PropTypes.object.isRequired,
  options: PropTypes.array.isRequired,
}

export default FormSelect
