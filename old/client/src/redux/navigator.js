import Home from '../react/Home'
import Boards from '../react/boards/Boards'
import Bookmarks from '../react/bookmarks/Bookmarks'
import Checklists from '../react/checklists/Checklists'
import Commands from '../react/commands/Commands'
import Feeds from '../react/feeds/Feeds'
import Financial from '../react/financial/Financial'
import Health from '../react/health/Health'
import Jobs from '../react/jobs/Jobs'
import Journal from '../react/journal/Journal'
import Notes from '../react/notes/Notes'
import Settings from '../react/settings/Settings'
import Goals from '../react/goals/Goals'

export const RoutesMap = [
  {
    path: '/',
    name: 'home',
    label: 'Home',
    icon: 'home',
    main: true,
    component: Home,
  },
  {
    path: '/boards',
    name: 'boards',
    label: 'Boards',
    icon: 'tasks',
    main: true,
    component: Boards,
  },
  {
    path: '/journal',
    name: 'journal',
    label: 'Journal',
    icon: 'pencil',
    main: true,
    component: Journal,
  },
  {
    path: '/notes',
    name: 'notes',
    label: 'Notes',
    icon: 'sticky-note',
    main: true,
    component: Notes,
  },
  {
    path: '/checklists',
    name: 'checklists',
    label: 'Checklists',
    icon: 'check-circle-o',
    main: true,
    component: Checklists,
  },
  {
    path: '/health',
    name: 'health',
    label: 'Health',
    icon: 'medkit',
    main: true,
    component: Health,
  },
  {
    path: '/financial',
    name: 'financial',
    label: 'Financial',
    icon: 'dollar',
    main: true,
    component: Financial,
  },
  {
    path: '/bookmarks',
    name: 'bookmarks',
    label: 'Bookmarks',
    icon: 'bookmark-o',
    main: true,
    component: Bookmarks,
  },
  {
    path: '/feeds',
    name: 'feeds',
    label: 'Feeds',
    icon: 'rss-square',
    main: true,
    component: Feeds,
  },
  {
    path: '/goals',
    name: 'goals',
    label: 'Goals',
    icon: 'gear',
    main: true,
    component: Goals,
  },
  {
    path: '/commands',
    name: 'commands',
    label: 'Commands',
    icon: 'magic',
    main: true,
    component: Commands,
  },
  {
    path: '/jobs',
    name: 'jobs',
    label: 'Jobs',
    icon: 'gavel',
    main: true,
    component: Jobs,
  },
  {
    path: '/settings',
    name: 'settings',
    label: 'Settings',
    icon: 'gear',
    main: true,
    component: Settings,
  },
]

export const MainRoutes = RoutesMap.filter(r => r.main)
