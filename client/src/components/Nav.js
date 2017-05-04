var React = require('react');
var NavLink = require('react-router-dom').NavLink;

function Nav(){
    return (
        <ul className='nav'>
            <li><NavLink exact activeClassName='active' to='/'>Apollo</NavLink></li>
            <li><NavLink activeClassName='active' to='/journal'>Journal</NavLink></li>
        </ul>
    )
}

module.exports = Nav;
