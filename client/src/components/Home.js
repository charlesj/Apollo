var React = require('react');

var HomeHealth = require('./health/HomeHealth');

class Home extends React.Component {
  render() {
    return (
      <div>
        <h2>Home</h2>
        <HomeHealth />
      </div>
    )
  }
}

module.exports = Home;
