import { useRef } from 'react';
import { FaPlus } from 'react-icons/fa';

const AddNote = ({ handleSubmit, newNote, setNewNote }) => {
  const inputRef = useRef();

  return (
    <form className='addForm' onSubmit={handleSubmit}>
      <label htmlFor='addNote'>Add Note</label>
      <input
        autoFocus
        ref={inputRef}
        id='addNote'
        type='text'
        placeholder='Add Note'
        required
        value={newNote}
        onChange={(e) => setNewNote(e.target.value)}
      />
      <button
        type='submit'
        aria-label='Add Note'
        onClick={() => inputRef.current.focus()}
      >
        <FaPlus />
      </button>
    </form>
  );
};

export default AddNote;