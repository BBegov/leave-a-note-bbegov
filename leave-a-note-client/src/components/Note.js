import { useState } from 'react';
import { FiDelete, FiEdit } from 'react-icons/fi';
import UpdateNote from './UpdateNote';

const Note = ({ note, handleDelete, handleUpdate }) => {
    const date = new Date(note.publishDate);
    const dateTime = `${date.toLocaleDateString('hu-HU')} ${date.toLocaleTimeString('hu-HU')}`;
    const [updateNote, setUpdateNote] = useState('');

    return (
        <div key={note.id} className='note'>
            <div>
                <div className='noteHeader'>
                    <p>{dateTime}</p>
                    <p>by User {note.userId}</p>
                </div>
                {updateNote ? (
                    <UpdateNote
                        updateNote={updateNote}
                        setUpdateNote={setUpdateNote}
                        handleUpdate={handleUpdate}
                    />) : (
                    <p>{note.noteText}</p>
                )}
            </div>
            <div className='lineButtonsContainer'>
                <FiEdit
                    role='button'
                    className='updateButton'
                    tabIndex='0'
                    onClick={() => setUpdateNote(note.noteText)}
                    aria-label={`Update ${note.noteText}`}
                />
                <FiDelete
                    role="button"
                    className="deleteButton"
                    tabIndex="0"
                    onClick={() => handleDelete(note.id)}
                    aria-label={`Delete ${note.noteText}`}
                />
            </div>
        </div>
    );
};

export default Note;