import { FiDelete } from 'react-icons/fi';

const Note = ({ myKey: idForKey, note, handleDelete }) => {
    const date = new Date(note.publishDate);
    const dateTime = `${date.toLocaleDateString('hu-HU')} ${date.toLocaleTimeString('hu-HU')}`;

    return (
        <div key={idForKey} className='note'>
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
                onClick={() => handleDelete(note.id)}
            />
        </div>
    );
};

export default Note;