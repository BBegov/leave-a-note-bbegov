import { useState } from 'react';
import axios from 'axios';
import useAxiosFetch from '../hooks/useAxiosFetch';
import SearchNote from './SearchNote';
import Notes from './Notes';
import AddNote from './AddNote';

const Home = ({ url }) => {
    const { data: notes, setData: setNotes, isLoading, fetchError } = useAxiosFetch(url);
    const [search, setSearch] = useState('');
    const [newNote, setNewNote] = useState('');

    const handleSubmit = (e) => {
        e.preventDefault();
        if (!newNote) return;

        axios.post(url, {
            noteText: newNote,
            userId: 1
        })
            .then(function (response) {
                setNotes([response.data, ...notes]);
            })
            .catch(function (error) {
                console.log(error);
            });
        setNewNote('');
    };

    const handleDelete = async (id) => {
        setNotes(notes.filter(note => note.id !== id));
        await axios.delete(`${url}/${id}`);
    };

    const handleUpdate = async (id, updateNote) => {
        const response = await axios.put(`${url}/${id}`, {
            noteText: updateNote,
            userId: 1
        });

        setNotes([response.data, ...(notes.filter(note => note.id !== id))]);
    };

    return (
        <div className='notes'>
            <AddNote
                handleSubmit={handleSubmit}
                newNote={newNote}
                setNewNote={setNewNote}
            />
            <SearchNote
                search={search}
                setSearch={setSearch}
            />
            <Notes
                isLoading={isLoading}
                fetchError={fetchError}
                notes={notes}
                search={search}
                handleDelete={handleDelete}
                handleUpdate={handleUpdate}
            />
        </div >
    );
};

export default Home;