var React = require('react');

var HomeHealth = require('./health/HomeHealth');

class Home extends React.Component {
  render() {
    return (
      <div>
        <HomeHealth />
      </div>
    )
  }
}

module.exports = Home;
