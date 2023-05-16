import '@material-ui/core/styles';

declare module '@mui/system/styles/createPalette' {
  interface Palette {
    myCustomColor?: Palette['primary'];
  }
  interface PaletteOptions {
    myCustomColor?: PaletteOptions['primary'];
  }
}