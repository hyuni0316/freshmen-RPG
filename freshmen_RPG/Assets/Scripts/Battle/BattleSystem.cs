using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public enum BattleState { Start, PlayerTurn, EnemyTurn, Busy }

public class BattleSystem : MonoBehaviour
{
    [SerializeField] private PlayerUnit player;
    [SerializeField] private EnemyHud enemyHud;
    [SerializeField] private EnemyUnit enemyUnit;
    
    [SerializeField] private BattleDialogBox dialogBox;
    [SerializeField] private GameObject blocker;
    [SerializeField] private GameObject runawayUI;
    
    private BattleState state;
    private bool isMonsterDefeated = false;
    private bool isButtonClicked = false;
    private string playerChoice;
    private bool isGameOver = false;
    
    // quiz
    [SerializeField] private GameObject quizUI;
    [SerializeField] private TextMeshProUGUI quizText;
    private int OXquizNum = -1;
    private int quizAnswer = -1;
    [SerializeField] private string[] _quizArr = new string[3];
    [SerializeField] private int[] _quizAnswerArr = new int[3];
    
    void Start()
    {
        StartCoroutine(SetupBattle()); 
        StartBattle();
    }

    void StartBattle()
    {
        StartCoroutine(coFunc_RollTurns());
    }

    IEnumerator coFunc_RollTurns()
    {
        for (int round = 0;; round++)
        {
            // 플레이어 턴
            yield return new WaitUntil(() => state == BattleState.PlayerTurn);
            yield return PlayerTurn();
            yield return new WaitForSeconds(0.5f);
            yield return new WaitUntil(() => isButtonClicked == true);
            yield return AfterPlayerTurn(playerChoice);
            yield return new WaitForSeconds(1f);
            if (isMonsterDefeated)
            {
                GameClear();
                yield break;
            }
            
            // 적 몬스터 턴
            yield return EnemyTurn();
            yield return new WaitForSeconds(1f);
            AfterEnemyTurn();
            if (isGameOver)
            {
                GameOver();
                yield break;
            }
        }
    }

    public IEnumerator SetupBattle()
    {
        Debug.Log("SetupBattle");
        blocker.SetActive(true);
        player.Setup();
        enemyUnit.SetUp();
        enemyHud.SetData(enemyUnit._Monster);

        yield return dialogBox.TypeDialog($"공학관에 {enemyUnit._Monster._monsterName}가 나타났습니다.\n적을 물리쳐 공학관의 평화를 지켜주세요.");
        yield return new WaitForSeconds(0.5f);
        state = BattleState.PlayerTurn;
    }

    IEnumerator PlayerTurn()
    {
        Debug.Log("PlayerTurn");
        state = BattleState.PlayerTurn;
        yield return dialogBox.TypeDialog("무엇을 할까요?");
        blocker.SetActive(false);
    }

    public void ButtonClicked(string selection)
    {
        Debug.Log("ButtonClicked");
        if (state != BattleState.PlayerTurn) return;
        
        playerChoice = selection;
        isButtonClicked = true;
    }
    
    IEnumerator AfterPlayerTurn(string selection)
    {
        Debug.Log("AfterPlayerTurn");
        state = BattleState.Busy;
        switch (selection)
        {
            case "first skill":
                yield return FirstSkill();
                break;
            case "second skill":
                break;
            case "third skill":
                break;
            case "forth skill":
                break;
            case "runaway":
                yield return RunAway();
                break;
        }
        yield return enemyHud.UpdateHP();
        if (OXquizNum != -1)
        {
            yield return OXQuiz();
        }
        state = BattleState.EnemyTurn;
        isButtonClicked = false;
    }

    IEnumerator OXQuiz()
    {
        quizUI.SetActive(true);
        yield return dialogBox.TypeDialog($"{enemyUnit._Monster._monsterName}의 저항이 거세집니다.");
        yield return dialogBox.TypeDialog($"{enemyUnit._Monster._monsterName}의 저항을 잠재우기 위해서는 퀴즈를 맞혀 기선을 제압해야해요.");
        
        yield return new WaitUntil(() => quizAnswer != -1);

        if (quizAnswer == _quizAnswerArr[OXquizNum])
        {
            yield return dialogBox.TypeDialog($"{enemyUnit._Monster._monsterName}를 진정시켰습니다. 공격력이 올라가지 않습니다.");
        }
        else
        {
            yield return dialogBox.TypeDialog(
                $"{enemyUnit._Monster._monsterName}를 진정시키지 못했습니다. {enemyUnit._Monster._monsterName}의 공격력이 5 올라갑니다.");
            enemyUnit._Monster.Attack += 5;
        }
        quizUI.SetActive(false);
    }

    public void QuizAnser_Onclicked(int response)
    {
        quizAnswer = response;
    }
    
    IEnumerator FirstSkill()
    {
        Debug.Log("First Skill");
        int skillDamage = 30;
        yield return dialogBox.TypeDialog($"플레이어의 과제 투척 공격!");
        yield return new WaitForSeconds(0.5f);
        int realDamage = enemyUnit._Monster.HP;
        OXquizNum = enemyUnit._Monster.TakeDamage(player.Attack, skillDamage);
        realDamage -= enemyUnit._Monster.HP;
        yield return dialogBox.TypeDialog($"{enemyUnit._Monster._monsterName}은 {realDamage}의 데미지를 받았다.");
    }

    public IEnumerator RunAway()
    {
        Debug.Log("Run away");
        dialogBox.ResetDialog();
        yield return dialogBox.TypeDialog("무사히 도망쳤습니다.");
        SceneManager.LoadScene("Scenes/MainScene");
    }

    IEnumerator EnemyTurn()
    {
        Debug.Log("Enemy Turn");
        state = BattleState.EnemyTurn;
        yield return dialogBox.TypeDialog($"{enemyUnit._Monster._monsterName} 가 공격합니다.");
        int realDamage = player.HP;
        isGameOver = player.TakeDamage(10, 20);
        realDamage -= player.HP;
        yield return dialogBox.TypeDialog($"플레이어는 {realDamage}의 데미지를 받았다.");
        yield return player.UpdateHP();
    }

    public void AfterEnemyTurn()
    {
        Debug.Log("AfterEnemyTurn");
        state = BattleState.PlayerTurn;
    }

    public void TryRunaway_Onclicked()
    {
        runawayUI.SetActive(true);
    }
    
    IEnumerator GameClear()
    {
        // TODO
        Debug.Log("GameClear");
        yield return dialogBox.TypeDialog("적을 무찔렀습니다!");
        // 페이드 아웃되면서 다음 화면으로
    }

    IEnumerator GameOver()
    {
        Debug.Log("Gameover");
        yield return dialogBox.TypeDialog("쓰러지고 말았습니다...");
        // 페이드 아웃되면서 메인화면으로
    }
}
