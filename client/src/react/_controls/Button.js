import React from "react";
import PropTypes from "prop-types";
import ClassNames from "classnames";
import "./Button.css";

function Button(props) {
  const { onClick, children, className, primary, type } = props;

  return (
    <button
      className={ClassNames({
        button: true,
        "button-primary": primary,
        [className]: true
      })}
      type={type}
      onClick={onClick}
    >
      {children}
    </button>
  );
}

Button.propTypes = {
  onClick: PropTypes.func,
  primary: PropTypes.bool,
  type: PropTypes.string
};

export default Button;
