import { Box, IconButton, TextField } from "@mui/material";
import { useCallback, useState } from "react";

import AddIcon from "@mui/icons-material/Add";
import RemoveIcon from "@mui/icons-material/Remove";

interface AppQtyCounterProps {
  minVal?: number;
  initValue?: number;
  onChange?: (newValue: number) => void;
}
export const AppQtyCounter = ({
  minVal = 1,
  initValue = 1,
  onChange,
}: AppQtyCounterProps) => {
  const [value, setValue] = useState(initValue);

  const updateValue = useCallback(
    (newValue: number) => {
      setValue(newValue);
      onChange?.(newValue);
    },
    [onChange, setValue]
  );

  return (
    <Box
      sx={{
        m: 1,
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
      }}
    >
      <IconButton
        onClick={() => {
          if (value > minVal) updateValue(value - 1);
        }}
        size="small"
      >
        <RemoveIcon />
      </IconButton>
      <TextField
        size="small"
        // type="number"
        value={value}
        onChange={(e) => {
          const regex = /^[0-9\b]+$/;
          if (e.target.value === "" || regex.test(e.target.value)) {
            const n = Number(e.target.value);
            if (n > 0) updateValue(n);
          }
        }}
        inputProps={{ min: 0, style: { textAlign: "center" } }}
        sx={{ width: 60 }}
      />
      <IconButton
        onClick={() => {
          updateValue(value + 1);
        }}
        size="small"
      >
        <AddIcon />
      </IconButton>
    </Box>
  );
};
