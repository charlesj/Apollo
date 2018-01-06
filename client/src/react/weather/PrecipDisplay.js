import React from 'react';
import PropTypes from 'prop-types';

function PrecipDisplay(props) {
  var percentDisplay = Math.round(props.chance * 100);
  if (percentDisplay === 0) {
    return <div>No Precipitation</div>;
  }
  return (
    <div>
      {percentDisplay}% chance for {props.precipType}
    </div>
  );
}

PrecipDisplay.propTypes = {
  chance: PropTypes.number.isRequired,
  precipType: PropTypes.string,
}

export default PrecipDisplay;
