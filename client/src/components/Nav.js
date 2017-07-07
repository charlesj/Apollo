import React from 'react';
import { NavLink } from 'react-router-dom';
import FontAwesome from 'react-fontawesome';

function Nav() {
  return (
    <nav>
      <NavLink exact activeClassName='active' to='/'><FontAwesome name='home' size='1x' /> Home</NavLink>
      <NavLink activeClassName='active' to='/todo'><FontAwesome name='check-square-o' size='1x' /> Organize</NavLink>
      <NavLink activeClassName='active' to='/journal'><FontAwesome name='pencil' size='1x' /> Log</NavLink>
      <NavLink activeClassName='active' to='/health'><FontAwesome name='medkit' size='1x' /> Health</NavLink>
      <NavLink activeClassName='active' to='/bookmarks'><FontAwesome name='bookmark-o' size='1x' /> Bookmarks</NavLink>
      <NavLink activeClassName='active' to='/settings'><FontAwesome name='gear' size='1x' /> Settings</NavLink>
    </nav>
  )
}

module.exports = Nav;
