
import { red } from '@material-ui/core/colors';
import { createMuiTheme } from '@material-ui/core/styles';

export const theme = createMuiTheme({
  palette: {
    primary: {
      main: '#008FEC',
    },
    secondary: {
      main: '#FFE14E',
    },
    error: {
      main: red.A400,
    },
    background: {
      default: '#fff',
    },
  },
})
