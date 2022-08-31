import React, { useRef } from 'react';
import { FiPlusSquare } from 'react-icons/fi';
import { MdOutlineCancel } from 'react-icons/md';

const UpdateNote = ({ id, updateNote, setUpdateNote, handleUpdate }) => {
    const inputRef = useRef();

    return (
        <form className='updateForm' onSubmit={(e) => e.preventDefault()}>
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
            <FiPlusSquare
                role='button'
                className='confirmButton'
                tabIndex='0'
                onClick={() => { handleUpdate(id, updateNote); setUpdateNote(''); }}
                aria-label={'Confirm update'}
            />
            <MdOutlineCancel
                role='button'
                className='cancelButton'
                tabIndex='0'
                onClick={() => setUpdateNote('')}
                aria-label={'Cancel update'}
            />
        </form>
    );
};

export default UpdateNote;