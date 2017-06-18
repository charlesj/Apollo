var React = require('react');
var NavLink = require('react-router-dom').NavLink;
var FontAwesome = require('react-fontawesome');

function Nav() {
  return (
    <nav>
        <div>
            <div>
            <NavLink exact activeClassName='active' to='/'><FontAwesome name='home' size='2x' /></NavLink>
            <NavLink activeClassName='active' to='/journal'><FontAwesome name='list' size='2x' /></NavLink>
            <NavLink activeClassName='active' to='/health'><FontAwesome name='medkit' size='2x' /></NavLink>
            <NavLink activeClassName='active' to='/settings'><FontAwesome name='gear' size='2x' /></NavLink>
            </div>
        </div>
    </nav>
  )
}

module.exports = Nav;
