import { createTheme } from '@mui/material/styles';
import { orange, blue, grey } from '@mui/material/colors';

export const theme = createTheme({
  palette: {
    primary: {
      main: orange[500],
    },
    secondary: {
      main: blue[500],
    },
    background: {
      default: grey[100]
    }
  },
  spacing: 8,
  shape: {
    borderRadius: 8
  }
});
