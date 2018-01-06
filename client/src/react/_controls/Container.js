import React from "react";
import PropTypes from "prop-types";
import ClassNames from "classnames";
import "./Container.css";

function Container(props) {
  const { children, grow, className, width } = props;
  let styles = {};
  if (width) {
    styles = {
      width: width + "px"
    };
  }

  return (
    <div
      className={ClassNames({
        container: true,
        "container-grow": grow,
        [className]: true
      })}
      style={styles}
    >
      {children}
    </div>
  );
}

Container.propTypes = {
  width: PropTypes.number
};
export default Container;
