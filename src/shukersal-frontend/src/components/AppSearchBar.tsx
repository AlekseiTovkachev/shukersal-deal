import React, { useState, useCallback } from 'react';
import { Paper, InputBase, IconButton } from '@mui/material';

import SearchIcon from '@mui/icons-material/Search';

interface AppSearchBarProps {
    onChange?: (newSearchText: string) => void;
    onSearch?: (searchText: string) => void;
}

export const AppSearchBar = ({ onChange, onSearch }: AppSearchBarProps) => {
    const [searchText, setSearchText] = useState<string>('');

    const handleChange = useCallback<React.ChangeEventHandler<HTMLTextAreaElement | HTMLInputElement>>((e) => {
        const newValue = e.target.value;
        setSearchText(newValue);
        onChange?.(newValue);
    }, [onChange, setSearchText]);

    const handleSubmit = useCallback(() => {
        onSearch?.(searchText);
    }, [onSearch, searchText]);

    return (
        <Paper
            component="form"
            sx={{ display: 'flex', alignItems: 'center', width: '100%' }}
        >
            <InputBase
                sx={{ ml: 1, flex: 1 }}
                placeholder="Search Shukersal"
                onChange={handleChange}
                onSubmit={(e) => {
                    e.preventDefault();
                    handleSubmit();
                }}
            />
            <IconButton onClick={handleSubmit} type="button" sx={{ p: '10px' }}>
                <SearchIcon />
            </IconButton>
        </Paper>
    );
}