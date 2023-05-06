import { createTheme } from '@mui/material/styles';
import { orange, blue, grey } from '@mui/material/colors';

export const theme = createTheme({
  palette: {
    primary: {
      main: '#263238',
    },
    secondary: {
      main: '#F4D35E',
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
