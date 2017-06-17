var React = require('react');
var NavLink = require('react-router-dom').NavLink;

function Nav() {
  return (
    <nav>
        <div>
            <div className='navHeader'>Apollo</div>
            <div>
            <NavLink exact activeClassName='active' to='/'>Home</NavLink>
            <NavLink activeClassName='active' to='/journal'>Journal</NavLink>
            <NavLink activeClassName='active' to='/health'>Health</NavLink>
            <NavLink activeClassName='active' to='/settings'>Settings</NavLink>
            </div>
        </div>
    </nav>
  )
}

module.exports = Nav;
