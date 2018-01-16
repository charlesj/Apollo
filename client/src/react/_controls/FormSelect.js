import React from "react";

import "./FormSelect.css";

function FormSelect(props) {
  const { input, options } = props;

  return (
    <select {...input}>
      {options.map(opt => {
        return (
          <option key={opt.value} value={opt.value}>
            {opt.label}
          </option>
        );
      })}
    </select>
  );
}

export default FormSelect;
