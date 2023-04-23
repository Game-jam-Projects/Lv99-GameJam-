using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragAndDropManager : Singleton<DragAndDropManager>
{
    [SerializeField] private int minVal = 0;
    [SerializeField] private int maxVal = 9;
    [SerializeField] private List<Sprite> listHint; //  Lista onde armazena os sprites de dica do puzzle
    [SerializeField] private List<Image> hintImagesUI;
    [Header("lista de ids que corresponde as celulas que o jogador colocou para resolver o puzzle")]
    public List<IdsCellsFinal> idsCellsFinal = new(); //lista de ids que corresponde as celulas que o jogador colocou para resolver o puzzle
    [Header("Ids que foram sorteados para resolver o puzzle")]
    public List<int> idsPuzzle = new();
    public List<InfosTargetSlot> targetSlots;
    public InputReference inputReference;

    public GameObject play;
    public GameObject win;

    // Start is called before the first frame update
    void Start()
    {
        inputReference = GetComponent<InputReference>();
        RandomizeIds();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void RandomizeIds()
    {
        // Crie uma lista para conter os n�meros inteiros selecionados aleatoriamente
        List<int> selectedIds = new List<int>();

        // Repetir tr�s vezes para selecionar tr�s n�meros inteiros �nicos
        for (int i = 0; i < 3; i++)
        {
            int randInt;
            do
            {
                // Gera um n�mero inteiro aleat�rio dentro do intervalo especificado
                randInt = UnityEngine.Random.Range(minVal, maxVal + 1);
            } while (selectedIds.Contains(randInt)); // Verifique se o n�mero inteiro j� n�o est� na lista

            // Adicione o inteiro selecionado � lista
            selectedIds.Add(randInt);
        }

        // Adicione os n�meros inteiros selecionados � lista existente de IDs de quebra-cabe�a
        for (int i = 0; i < selectedIds.Count; i++)
        {
            targetSlots[i].idCorrect = selectedIds[i];
            hintImagesUI[i].sprite = listHint[selectedIds[i]];
        }

    }

    public void CheckCompletePuzzle()
    {
        if (VerifyPuzzleSolution())
        {
            Debug.Log("Deu Bom!!!");
            GameManager.Instance.Puzzle2 = true;
            CheckPuzzle.Instance.Puzzle2[0].SetActive(false);
            CheckPuzzle.Instance.Puzzle2[1].SetActive(true);
            play.SetActive(false);
            win.SetActive(true);
            GameManager.Instance.PuzzleComplete();
        }
        else
        {
            Debug.Log("RUIIM...");
        }
    }

    public bool VerifyPuzzleSolution()
    {
        // Verifica se as listas t�m o mesmo tamanho
        //if (idsPuzzle.Count != idsCellsFinal.Count)
        //{
        //    return false;
        //}

        // Percorre cada elemento das duas listas ao mesmo tempo
        for (int i = 0; i < targetSlots.Count; i++)
        {
            int puzzleNumber = targetSlots[i].idCorrect;
            IdsCellsFinal cellFinal = idsCellsFinal[i];

            // Verifica se o n�mero do puzzle � igual � c�lula final correspondente
            if (puzzleNumber != cellFinal.idCell)
            {
                return false;
            }

            // Verifica se a posi��o do puzzle � igual ao target da c�lula final correspondente
            if (i != cellFinal.idTarget)
            {
                return false;
            }
        }

        // Se chegou at� aqui, � porque a solu��o est� correta
        return true;
    }

    public void AddIdListCells(int idCell, int idTarget)
    {
        bool isContain = false;
        foreach (var cell in idsCellsFinal)
        {
            if(cell.idCell == idCell)
            {
                isContain = true;
                break;
            }
        }

        if(isContain == false)
        {
            idsCellsFinal.Add(new IdsCellsFinal(idCell, idTarget));
        }

        if(idsCellsFinal.Count >= 3)
        {
            CheckCompletePuzzle();
        }
    }

    public void RemoveListCells(int idCell, int idTarget)
    {
        for (int i = 0; i < idsCellsFinal.Count; i++)
        {
            if (idsCellsFinal[i].idCell == idCell && idsCellsFinal[i].idTarget == idTarget)
            {
                idsCellsFinal.Remove(idsCellsFinal[i]);
            }
        }
    }
}

[System.Serializable]
public class InfosTargetSlot
{
    public GameObject target; 
    public int idCorrect; // ID CORRETO PARA ESTE PUZZLE
    public bool isOccupied; // EST� OCUPADO
}


[System.Serializable]
public class IdsCellsFinal
{
    public int idCell;
    public int idTarget;

    public IdsCellsFinal(int _idCell, int _idTarget)
    {
        idCell = _idCell;
        idTarget = _idTarget;
    }
}
