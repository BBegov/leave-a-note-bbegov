import { FiDelete } from 'react-icons/fi';

const Note = ({ note, handleDelete }) => {
    const date = new Date(note.publishDate);
    const dateTime = `${date.toLocaleDateString('hu-HU')} ${date.toLocaleTimeString('hu-HU')}`;

    return (
        <div key={note.id} className='note'>
            <div>
                <div className='noteHeader'>
                    <p>{dateTime}</p>
                    <p>by User {note.userId}</p>
                </div>
                <p>{note.noteText}</p>
            </div>
            <FiDelete
                role="button"
                className="deleteButton"
                tabIndex="0"
                onClick={() => handleDelete(note.id)}
                aria-label={`Delete ${note.noteText}`}
            />
        </div>
    );
};

export default Note;