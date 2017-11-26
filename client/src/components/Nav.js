import React from 'react';
import { Link } from 'react-router-dom';
import ReactModal from 'react-modal';
import FontAwesome from 'react-fontawesome';
import HotKey from 'react-shortcut';
import ServerInfo from './ServerInfo';
import { withRouter } from 'react-router-dom';

function ChooseableLink(props) {

  var className = '';
  if (props.selected) {
    className = 'selected';
  }

  return (<div className={className}>
    {props.children}
  </div>)
}

class Nav extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      navOpen: false,
      selectedLinkIndex: 0
    }

    this.toggleOverlay = this.toggleOverlay.bind(this);
    this.moveSelectionDown = this.moveSelectionDown.bind(this);
    this.moveSelectionUp = this.moveSelectionUp.bind(this);
    this.navigate = this.navigate.bind(this);
  }

  toggleOverlay() {
    this.setState({
      navOpen: !this.state.navOpen
    });
  }

  links =[
    {
      href: "/",
      label: "Home",
      icon: "home"
    },
    {
      href: "/boards",
      label: "Boards",
      icon: "tasks"
    },
    {
      href: "/journal",
      label: "Log",
      icon: "pencil"
    },
    {
      href: "/notebooks",
      label: "Notes",
      icon: "sticky-note"
    },
    {
      href: "/checklists",
      label: "Checklists",
      icon: "check-circle-o"
    },
    {
      href: "/health",
      label: "Health",
      icon: "medkit"
    },
    {
      href: "/financial",
      label: "Financial",
      icon: "dollar"
    },
    {
      href: "/bookmarks",
      label: "Bookmarks",
      icon: "bookmark-o"
    },
    {
      href: "/feeds",
      label: "Feeds",
      icon: "rss-square"
    },
    {
      href: "/jobs",
      label: "Jobs",
      icon: "gavel"
    },
    {
      href: "/settings",
      label: "Settings",
      icon: "gear"
    }
  ];

  moveSelectionUp() {
    if (this.state.selectedLinkIndex > 0) {
      this.setState({
        selectedLinkIndex: this.state.selectedLinkIndex - 1
      });
    }
  }

  moveSelectionDown() {
    if (this.state.selectedLinkIndex < this.links.length - 1) {
      this.setState({
        selectedLinkIndex: this.state.selectedLinkIndex + 1
      });
    }
  }

  navigate() {
    this.toggleOverlay();
    this.props.history.push(this.links[this.state.selectedLinkIndex].href);
  }

  render() {
    return (
      <div>
        <HotKey
      keys={['escape']}
      onKeysCoincide={this.toggleOverlay}
      />
        <ReactModal
      contentLabel="Navigation"
      className="navContainer"
      isOpen={ this.state.navOpen } >
          <div>
        <HotKey
      keys={['n']}
      onKeysCoincide={this.moveSelectionDown}
      />
        <HotKey
      keys={['m']}
      onKeysCoincide={this.moveSelectionUp}
      />

      <HotKey
      keys={['enter']}
      onKeysCoincide={this.navigate}
      />
      <nav>
        { this.links.map((l, i) => {
        return (<ChooseableLink key={i} selected={this.state.selectedLinkIndex === i}>
            <Link to={l.href} onClick={this.toggleOverlay}>
              <FontAwesome name={l.icon} /> {l.label}</Link>
            </ChooseableLink>)
      })}

      </nav>
          <ServerInfo /> 
          <div className="logoutButton">
          <a onClick={this.props.logout} className='textbutton pt-small'><FontAwesome name='sign-out' /> logout</a>
          </div>

          </div>
        </ReactModal>
      </div>
    )
  }
}

module.exports = withRouter(Nav);
