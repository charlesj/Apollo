var React = require('react');
var NavLink = require('react-router-dom').NavLink;

function Nav() {
  return (
    <nav className="pt-navbar">
        <div className="pt-navbar-group pt-align-left">
            <div className="pt-navbar-heading">Apollo</div>
            <div className="pt-navbar-group">
            <NavLink exact activeClassName='active' to='/' className='pt-button pt-minimal pt-icon-home'>Home</NavLink>
            <NavLink activeClassName='active' to='/journal' className='pt-button pt-minimal'>Journal</NavLink>
            <NavLink activeClassName='active' to='/health' className='pt-button pt-minimal'>Health</NavLink>
            <NavLink activeClassName='active' to='/settings' className='pt-button pt-minimal'>Settings</NavLink>
            </div>
        </div>
        </nav>
  )
}

module.exports = Nav;
