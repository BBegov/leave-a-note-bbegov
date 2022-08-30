import Note from './Note';

const Notes = ({
    isLoading, fetchError,
    notes,
    search,
    handleDelete,
    updateNote, setUpdateNote, handleUpdate }) => {
    return (
        <>
            {isLoading && <p className="statusMsg">Loading notes...</p>}
            {!isLoading && fetchError && <p className="statusMsg" style={{ color: "red" }}>{fetchError}</p>}
            {!isLoading && !fetchError && (notes.length ? (
                <>
                    {notes.filter(note => ((note.noteText).toLowerCase()).includes(search.toLowerCase())).map((note) => {
                        return (
                            <Note
                                key={note.id}
                                note={note}
                                handleDelete={handleDelete}
                                updateNote={updateNote}
                                setUpdateNote={setUpdateNote}
                                handleUpdate={handleUpdate}
                            />
                        );
                    })}
                </>
            ) : <p className="statusMsg">No notes to display.</p>)}
        </>
    );
};

export default Notes;