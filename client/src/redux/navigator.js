import Home from "../react/Home";
import Boards from "../react/boards/Boards";
import Bookmarks from "../react/bookmarks/Bookmarks";
import Checklists from "../react/checklists/Checklists";
import Commands from "../react/commands/Commands";
import Feeds from "../react/feeds/Feeds";
import Financial from "../react/financial/Financial";
import Health from "../react/health/Health";
import Jobs from "../react/jobs/Jobs";
import Log from "../react/log/Log";
import Notebooks from "../react/notebooks/Notebooks";
import Settings from "../react/settings/Settings";
import Goals from "../react/goals/Goals";

export const RoutesMap = [
  {
    path: "/",
    name: "home",
    label: "Home",
    icon: "home",
    component: Home
  },
  {
    path: "/boards",
    name: "boards",
    label: "Boards",
    icon: "tasks",
    component: Boards
  },
  {
    path: "/log",
    name: "log",
    label: "Log",
    icon: "pencil",
    component: Log
  },
  {
    path: "/notebooks",
    name: "notebooks",
    label: "Notebooks",
    icon: "sticky-note",
    component: Notebooks
  },
  {
    path: "/checklists",
    name: "checklists",
    label: "Checklists",
    icon: "check-circle-o",
    component: Checklists
  },
  {
    path: "/health",
    name: "health",
    label: "Health",
    icon: "medkit",
    component: Health
  },
  {
    path: "/financial",
    name: "financial",
    label: "Financial",
    icon: "dollar",
    component: Financial
  },
  {
    path: "/bookmarks",
    name: "bookmarks",
    label: "Bookmarks",
    icon: "bookmark-o",
    component: Bookmarks
  },
  {
    path: "/feeds",
    name: "feeds",
    label: "Feeds",
    icon: "rss-square",
    component: Feeds
  },
  {
    path: "/commands",
    name: "commands",
    label: "Commands",
    icon: "magic",
    component: Commands
  },
  {
    path: "/jobs",
    name: "jobs",
    label: "Jobs",
    icon: "gavel",
    component: Jobs
  },
  {
    path: "/settings",
    name: "settings",
    label: "Settings",
    icon: "gear",
    component: Settings
  },
  {
    path: "/goals",
    name: "goals",
    label: "Goals",
    icon: "gear",
    component: Goals
  }
];
