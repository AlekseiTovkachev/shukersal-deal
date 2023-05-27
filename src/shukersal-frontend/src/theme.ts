import { createTheme } from '@mui/material/styles';
import { orange, blue, grey } from '@mui/material/colors';

// Utility
function makeColorLighter(hexColor: string, percent: number) {
  // Parse the hex color code to RGB format
  const hex = hexColor.replace('#', '');
  const r = parseInt(hex.substring(0, 2), 16);
  const g = parseInt(hex.substring(2, 4), 16);
  const b = parseInt(hex.substring(4, 6), 16);

  // Increase the RGB values to make the color lighter
  const increment = Math.round(2.55 * percent);
  const newR = Math.min(255, r + increment);
  const newG = Math.min(255, g + increment);
  const newB = Math.min(255, b + increment);

  // Convert the new RGB values back to hex format
  const newHex = ((newR << 16) | (newG << 8) | newB).toString(16);
  console.log(`#${newHex.padStart(6, '0')}`);
  return `#${newHex.padStart(6, '0')}`;
}


export const theme = createTheme({
  palette: {
    primary: {
      main: '#263238',
    },
    secondary: {
      main: '#F4D35E',
    },
    background: {
      default: '#F2F9FA',
      
      //@ts-expect-error: Additional colors are supported by mui, this is for the typescript compiler
      default2: '#FEFEFE',
      artifacts: '#F9F9F9'
    }
  },
  spacing: 8,
  shape: {
    borderRadius: 12,
  },
  components: {
    MuiPaper: {
      defaultProps: {
        elevation: 0,
      },
    },
    MuiButton: {
      defaultProps: {
        disableElevation: true
      },
    },
  }
});
