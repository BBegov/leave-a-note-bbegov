import React, { useRef } from 'react';
import { FiPlusSquare } from 'react-icons/fi';

const UpdateNote = ({ id, updateNote, setUpdateNote, handleUpdate }) => {
    const inputRef = useRef();

    return (
        <form className='updateForm' onSubmit={(e) => { e.preventDefault(); handleUpdate(id) }}>
            <label htmlFor='updateNote'>Update Note</label>
            <input
                autoFocus
                ref={inputRef}
                id='updateNote'
                type='text'
                placeholder={updateNote}
                required
                value={updateNote}
                onChange={(e) => setUpdateNote(e.target.value)}
            />
            <button
                type='submit'
                aria-label='Update Note'
                onClick={() => inputRef.current.focus()}
            >
                <FiPlusSquare />
            </button>
        </form>
    );
};

export default UpdateNote;